from pywinauto import Application
import time
import os

BASE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
EXE = os.path.join(BASE_DIR, "Matchmaker", "bin", "Release", "net10.0-windows", "Matchmaker.exe")

def test_ui_launch():
    
    # verify file exists
    assert os.path.exists(EXE), f"Executable not found at {EXE}" # f tells interpreter it's a formatted string and to replace {exe_path} with the value of the variable exe_path

    # start application
    app = Application(backend="uia").start(EXE)
    window = app.window(title="Matchmaker")
    window.wait('visible', timeout=10)  # 10 second limit

    # confirm window exists
    assert window.exists(), "Main window did not appear"    # if failed, will raise an AssertionError with the message "Main window did not appear"

    app.kill()
