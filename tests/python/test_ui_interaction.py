from encodings import win32_code_page_search_function

from pywinauto import Application, Desktop
import time
import os
import pytest



BASE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", ".."))
EXE = os.path.join(BASE_DIR, "Matchmaker", "bin", "Release", "net10.0-windows", "Matchmaker.exe")

SAMPLE_DIR = os.path.abspath(os.path.join(os.path.dirname(__file__), "..", "sample_files"))

files = {
        "inovarBom": os.path.join(SAMPLE_DIR, "inovarBom.xlsx"),
        "cbom": os.path.join(SAMPLE_DIR, "cbom.xlsx"),
        "asBuilt": os.path.join(SAMPLE_DIR, "asBuilt.xlsx"),
        "faReport": os.path.join(SAMPLE_DIR, "faReport.pdf")
}

def open_file_dialog_and_select(path):

    # select file in standard windows openfiledialog
    dialog = Desktop(backend="uia").window(title_re="Open") # default windows dialog
    dialog.wait("visible", timeout=5)

    filename_edit = dialog.child_window(auto_id="1148", control_type="Edit")
    filename_edit.set_edit_text(path)

    open_button = dialog.child_window(title="Open", control_type="Button")
    open_button.click_input()

def test_ui_interaction():
    
    assert os.path.exists(EXE), "App not built"

    app = Application(backend="uia").start(EXE)
    win = app.window(title="Matchmaker")
    win.wait('visible', timeout=10)

    # Upload files
    win.btnInovarBOM.click_input()
    open_file_dialog_and_select(files["inovarBom"])

    win.btnCBOM.click_input()
    open_file_dialog_and_select(files["cbom"])

    win.btnAsBuilt.click_input()
    open_file_dialog_and_select(files["asBuilt"])

    win.btnFAReport.click_input()
    open_file_dialog_and_select(files["faReport"])

    # Verify labels change
    assert "inovarBom.xlsx" in win.lblInovarBOM.window_text(), "Inovar BOM label not updated"
    assert "cbom.xlsx" in win.lblCBOM.window_text(), "CBOM label not updated"
    assert "asBuilt.xlsx" in win.lblAsBuilt.window_text(), "As Built label not updated"
    assert "faReport.pdf" in win.lblFAReport.window_text(), "FA Report label not updated"

    # verify reset button enabled
    assert win.btnReset.is_enabled(), "Reset button not enabled after file upload"

    # clear individual file
    win.btnDeleteInovarBOM.click_input()
    assert "(no file selected)" in win.lblInovarBOM.window_text(), "Inovar BOM label not cleared after delete"

    # verify reset
    win.btnReset.click_input()
    assert "(no file selected)" in win.lblInovarBOM.window_text(), "Inovar BOM label not cleared after reset"
    assert "(no file selected)" in win.lblCBOM.window_text(), "CBOM label not cleared after reset"
    assert "(no file selected)" in win.lblAsBuilt.window_text(), "As Built label not cleared after reset"
    assert "(no file selected)" in win.lblFAReport.window_text(), "FA Report label not cleared after reset"

    # re-upload
    win.btnInovarBOM.click_input()
    open_file_dialog_and_select(files["inovarBom"])

    win.btnCBOM.click_input()
    open_file_dialog_and_select(files["cbom"])

    win.btnAsBuilt.click_input()
    open_file_dialog_and_select(files["asBuilt"])

    win.btnFAReport.click_input()
    open_file_dialog_and_select(files["faReport"])

    # verify "go"
    win.btnGo.click_input()

    app.kill()