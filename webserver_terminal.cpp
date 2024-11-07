#include "webserver_terminal.hpp"
#include "net/address_conversion.hpp"
#include <imgui.h>
#include <backends/imgui_impl_glfw.h>
#include <backends/imgui_impl_opengl3.h>
#include <format>
#include <thread>

webserver_terminal::webserver_terminal(webserver& webserver) noexcept : m_webserver{webserver}, m_running{true} {
 
}
webserver_terminal::~webserver_terminal() noexcept {

}

void webserver_terminal::run() {
 glfwInit();
 const char* glsl_version = "#version 130";
 glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
 glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
 this->m_window = glfwCreateWindow(1280, 720, "Terminal Window", nullptr, nullptr);
 glfwMakeContextCurrent(this->m_window);
 glfwSwapInterval(1);
 IMGUI_CHECKVERSION();
 ImGui::CreateContext();
 ImGui_ImplGlfw_InitForOpenGL(this->m_window, true);
 ImGui_ImplOpenGL3_Init(glsl_version);
 ImVec4 clear_color = ImVec4(0.45f, 0.55f, 0.60f, 1.00f);
 ImGuiIO& io = ImGui::GetIO();
 while (this->m_running && !glfwWindowShouldClose(this->m_window)) {
   glfwPollEvents();
   ImGui_ImplOpenGL3_NewFrame();
   ImGui_ImplGlfw_NewFrame();
   ImGui::NewFrame();
   io.DeltaTime = 1.0f / 60.0f;
   io.DisplaySize = ImVec2(1920, 1080);

   /* Scary Panel */
   {
    ImGui::Begin("Scary Panel");
    if (ImGui::Button("Shutdown")) {
     this->m_webserver.stop();
     this->m_running = false;
    }
    ImGui::End();
   }

   /* HTTP Client Threads (Compacted) */
   {
    ImGui::Begin("HTTP Client Threads (Compacted)", nullptr, ImGuiWindowFlags_HorizontalScrollbar);
    for (auto&& [wthread, client] : this->m_webserver.wthreads() | 
     std::views::transform([](auto&& wthread) noexcept { return std::make_tuple(&wthread, std::get<0>(wthread.args())); }) |
     std::views::filter([](auto&& wthread_client) noexcept { 
      return std::get<0>(wthread_client)->running() &&
             std::get<1>(wthread_client) != nullptr &&
             std::get<1>(wthread_client)->socket().socket_handle != NULL;
     }))
    {
     auto const connection_string = std::invoke([&]() noexcept {
      if (wthread->working()) { return std::format("Thread {} @ {}:{}", wthread->pid(), net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port); }
      else                    { return std::format("THread {}: N/A", wthread->pid()); }
     });
     ImGui::TextUnformatted(std::data(connection_string));
    }
    ImGui::End();
   }

   /* HTTP Client Threads (Full) */
   {
    ImGui::Begin("HTTP Client Threads (Full)", nullptr, ImGuiWindowFlags_HorizontalScrollbar);
    auto running_wthreads_view = this->m_webserver.wthreads() | std::views::transform([](auto&& wthread) noexcept { return wthread.running(); });
    auto working_wthreads_view = this->m_webserver.wthreads() | std::views::transform([](auto&& wthread) noexcept { 
     return wthread.working() && std::get<0>(wthread.args())->socket().socket_handle != NULL; 
    });
    auto const running_count = std::accumulate(
     std::begin(running_wthreads_view),
     std::end(running_wthreads_view),
     static_cast<std::size_t>(0));
    auto const working_count = std::accumulate(
     std::begin(working_wthreads_view),
     std::end(working_wthreads_view),
     static_cast<std::size_t>(0));
    auto const working_count_string = std::format("Current Active Threads: {}w/{}r/{}t", working_count, running_count, 1024);
    ImGui::TextUnformatted(std::data(working_count_string));
    for (std::size_t i = 0; auto&& [wthread, client] :  this->m_webserver.wthreads() | 
     std::views::transform([](auto&& wthread) noexcept { return std::make_tuple(&wthread, std::get<0>(wthread.args())); }) ) 
    {
     auto const http_client_message = std::invoke([&]() noexcept {
      if (wthread->working() && client->socket().socket_handle != NULL) { return std::format("- #{} Thread ({}): {}:{}", i, wthread->pid(), net::convert_ipv4_u32_to_string(client->socket().host), client->socket().port); }
      else if (wthread->running())                                      { return std::format("- #{} Thread ({}): N/A", i, wthread->pid()); }
      else                                                              { return std::format("- #{} Thread (N/A): N/A", i); }
     });
     ImGui::TextUnformatted(std::data(http_client_message));
     ++i;
    }
    ImGui::End();
   }

   /* MySQL Connections: Remote */
   {
    ImGui::Begin("MySQL Connections: Remote", nullptr, ImGuiWindowFlags_HorizontalScrollbar);
    auto const live_connection_count = std::accumulate(
     std::begin(this->m_webserver.thread_resource().remote_connection_pool.available()),
     std::end(this->m_webserver.thread_resource().remote_connection_pool.available()),
     static_cast<std::size_t>(0));
    auto const live_connection_count_string = std::format("Live Connection Count: {}", live_connection_count);
    ImGui::TextUnformatted(std::data(live_connection_count_string));
    for (auto&& [count, available] : std::views::enumerate(this->m_webserver.thread_resource().remote_connection_pool.available())) {
     auto const available_string = std::invoke([&]() noexcept {
      if (available) { return std::format("- {}: Available", count); }
      else           { return std::format("- {}: Unavailable", count); }
     });
     ImGui::TextUnformatted(std::data(available_string));
    }
    ImGui::End();
   }

   /* MySQL Connections: Local */
   {
    ImGui::Begin("MySQL Connections: Local", nullptr, ImGuiWindowFlags_HorizontalScrollbar);
    auto const live_connection_count = std::accumulate(
     std::begin(this->m_webserver.thread_resource().local_connection_pool.available()),
     std::end(this->m_webserver.thread_resource().local_connection_pool.available()),
     static_cast<std::size_t>(0));
    auto const live_connection_count_string = std::format("Live Connection Count: {}", live_connection_count);
    ImGui::TextUnformatted(std::data(live_connection_count_string));
    for (auto&& [count, available] : std::views::enumerate(this->m_webserver.thread_resource().local_connection_pool.available())) {
     auto const available_string = std::invoke([&](){
      if (available) { return std::format("- {}: Available", count); }
      else           { return std::format("- {}: Unavailable", count); }
     });
     ImGui::TextUnformatted(std::data(available_string));
    }
    ImGui::End();
   }
   
   ImGui::Render();
   int display_w, display_h;
   glfwGetFramebufferSize(this->m_window, &display_w, &display_h);
   glViewport(0, 0, display_w, display_h);
   glClearColor(clear_color.x * clear_color.w, clear_color.y * clear_color.w, clear_color.z * clear_color.w, clear_color.w);
   glClear(GL_COLOR_BUFFER_BIT);
   ImGui_ImplOpenGL3_RenderDrawData(ImGui::GetDrawData());
   glfwSwapBuffers(this->m_window);
 }
 ImGui_ImplOpenGL3_Shutdown();
 ImGui_ImplGlfw_Shutdown();
 ImGui::DestroyContext();
 glfwDestroyWindow(this->m_window);
 glfwTerminate();
}