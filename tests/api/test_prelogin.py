import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request(
 "http://ddnd.crabdance.com/prelogin",
 data=json.dumps({
  "Token": "cd02a0dd-36fd-4679-b754-6f24a4a70e14"
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
