import urllib.request
import json
import time
import threading

def run(i):
 time.sleep(i / 10)
 response = urllib.request.urlopen(urllib.request.Request(
  "http://ddnd.crabdance.com/login",
  data=json.dumps({
   "Email": "CaregiverEmail",
   "PasswordHash": "e7cf3ef4f17c3999a94f2c6f612e8a888e5b1026878e4e19398b23bd38ec221a"
  }).encode(),
  headers={"Content-Type": "application/json"},
  method="POST"
 ))
 print(f"Response #{i}: {response.read()}")


threads = [threading.Thread(target=run, args=(i,)) for i in range(512)]
for thread in threads:
 thread.start()
for thread in threads:
 thread.join()
