import subprocess
import os

solution_dir = ".."

host_project_dir = os.path.join(solution_dir, "RemoteControlPC")
host_bin_dir = os.path.join(host_project_dir, "bin/Release")


subprocess.run(["msbuild.exe", "RemoteControlPC.sln", "/t:Build", "/p:Configuration=Release"], cwd=solution_dir)


client_project_dir = os.path.join(solution_dir, "web_frontend")


subprocess.run(["npm.cmd", "run", "build"], cwd=client_project_dir)


client_build_dir = os.path.join(client_project_dir, "build")


final_build_dir = os.path.join(solution_dir, "build") 



