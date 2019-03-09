#include "windows.h"
#include "stdio.h"
#include "tchar.h"

int WINAPI WinMain(HINSTANCE inst, HINSTANCE prinst, LPSTR cmdline, int showcmd)
{
	int style;
	int exstyle;
	STARTUPINFO si;
	PROCESS_INFORMATION pi;
	LPTSTR cmd = _tcsdup(TEXT("C:\\Windows\\system32\\cmd.exe"));
	HWND win;
	HANDLE out;
	COORD size;
	SMALL_RECT area = {0,0,0,0};

	ZeroMemory(&si, sizeof(STARTUPINFO));

	CreateProcess(NULL, cmd, NULL, NULL, FALSE, NULL, NULL, NULL, &si, &pi);

	while(!AttachConsole(pi.dwProcessId))
	{
		Sleep(5);
	}
	SetConsoleTitle(L"preigniter");

	win = FindWindow(NULL, L"preigniter");
	style = GetWindowLong(win, GWL_STYLE);
	exstyle = GetWindowLong(win, GWL_EXSTYLE);
	SetWindowLong(win, GWL_STYLE, style & ~(WS_BORDER | WS_DLGFRAME | WS_SIZEBOX | WS_MAXIMIZE));
	SetWindowLong(win, GWL_EXSTYLE, exstyle & ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE));

	out = GetStdHandle(STD_OUTPUT_HANDLE);
	size = GetLargestConsoleWindowSize(out);

	area.Right = size.X;
	area.Bottom = size.Y;

	SetConsoleScreenBufferSize(out, size);
	SetConsoleWindowInfo(out, TRUE, &area);
	SetConsoleTextAttribute(out, FOREGROUND_RED | FOREGROUND_GREEN | FOREGROUND_INTENSITY);
	ShowWindow(win, SW_MAXIMIZE);
}
