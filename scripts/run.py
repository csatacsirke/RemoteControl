

import subprocess
import os

solution_dir = ".."

host_project_dir = os.path.join(solution_dir, "RemoteControlPC")
host_bin_dir = os.path.join(host_project_dir, "bin/Release")
host_exe_path = os.path.join(host_bin_dir, "RemoteControlPC.exe")


client_project_dir = os.path.join(solution_dir, "web_frontend")
client_build_dir = os.path.join(client_project_dir, "build")

# f:\Programming\RemoteControlPC\RemoteControlPC\bin\Release\RemoteControlPC.exe

host_process = subprocess.Popen(host_exe_path)
client_process = subprocess.Popen(["serve.cmd",  "-s",  "build"], cwd=client_project_dir)

#subprocess.run(host_exe_path)
#subprocess.run(["serve.cmd",  "-s",  "build"], cwd=client_project_dir)

host_process.wait()

client_process.terminate()
client_process.wait()



