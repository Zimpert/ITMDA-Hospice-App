import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "90b3a648-07a1-4fb4-a6b5-974cfb12a03a"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(response.read())

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "942d824c-0582-4399-8cb6-00a911998045"
 }).encode(),
 headers={"Content-Type": "application/json"},
 method="POST"
))
print(response.read())
