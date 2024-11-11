#pragma once

#include "types.hpp"
#include "webserver.hpp"
#include <gl/glew.h>
#include <glfw/glfw3.h>

class webserver_terminal {
public:
 webserver_terminal(webserver& webserver) noexcept;
 ~webserver_terminal() noexcept;

 void run();

private:
 webserver& m_webserver;
 GLFWwindow* m_window;
 bool m_running;
};