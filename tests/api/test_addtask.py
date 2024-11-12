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
   "http://ddnd.crabdance.com/addtask",
   data=json.dumps({
    "Token": client_info["Token"],
    "UserID": "42790224-5b78-498f-9755-9722b6cfd3ac",
    "Description": "This is my Test",
    "DateDue": "2024-12-25 12:05:19"
   }).encode(),
   headers={"Content-Type": "application/json"},
   method="POST"
  ))
 except:
  pass 

