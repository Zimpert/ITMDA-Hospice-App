import urllib.request
import json

response = urllib.request.urlopen(urllib.request.Request("http://ddnd.crabdance.com/thisismyrequest?a=19&b=testing", data=json.dumps({"data1": 15, "data2": {"var0": True, "y0rt": "my snort"}}).encode(), headers={"Content-Type": "application/json"}, method="POST"))
print(response.read())
