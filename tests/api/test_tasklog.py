import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/login",
 data=json.dumps({
  "Email": "bob.brown@example.com",
  "PasswordHash": "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
client_info = json.loads(response.read().decode())

if "Token" in client_info:
 try:
  response = urllib.request.urlopen(urllib.request.Request(
   "http://ddnd.crabdance.com/tasklog",
   data=json.dumps({
    "Token": client_info["Token"],
    "TaskID": "19a103e6-a0d1-11ef-a853-0ec51cb50a79"
   }).encode(),
   headers={"Content-Type": "application/json"},
   method="POST"
  ))
 except:
  pass


