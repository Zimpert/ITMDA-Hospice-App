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
 response = urllib.request.urlopen(urllib.request.Request(
  "http://ddnd.crabdance.com/medicine",
  data=json.dumps({
   "Token": client_info["Token"],
   "UserID": client_info["UserID"]
  }).encode(),
  headers={"Content-Type": "application/json"},
  method="POST"
 ))
 print(response.read())

