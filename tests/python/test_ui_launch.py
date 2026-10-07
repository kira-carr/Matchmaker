from readline import backend
from turtle import title

from pywinauto import Application
import time
import os

def test_ui_launch():
    
    # verify file exists
    exe_path = r"Matchmaker\bin\Release\net10.0-windows\Matchmaker.exe" # r tells interpreter it's raw string and to treat backslashes as literal rather than escape characters
    assert os.path.exists(exe_path), f"Executable not found at {exe_path}" # f tells interpreter it's a formatted string and to replace {exe_path} with the value of the variable exe_path

    # start application
    app = Application(backend="uia").start(exe_path)
    window = app.window(title="Matchmaker")
    window.wait('visible', timeout=10)  # 10 second limit

    # confirm window exists
    assert window.exists(), "Main window did not appear"    # if failed, will raise an AssertionError with the message "Main window did not appear"

    app.kill()
