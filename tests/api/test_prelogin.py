import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "29b717a4-15b7-46bd-9c62-aed7b88a8e22"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(response.read())

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "29b717a4-15b7-46bd-9c62-aed7b88a8e20"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(response.read())
