import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "a7c6849c-ea35-44a7-96bd-6f7681b8175a"
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
