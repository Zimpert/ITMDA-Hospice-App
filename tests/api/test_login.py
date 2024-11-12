import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/login",
 data=json.dumps({
  "Email": "CaregiverEmail",
  "PasswordHash": "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(json.dumps(json.loads(response.read().decode()), indent=1))

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/login",
 data=json.dumps({
  "Email": "CaregiverEmail",
  "PasswordHash": "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221b"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(json.dumps(json.loads(response.read().decode()), indent=1))
