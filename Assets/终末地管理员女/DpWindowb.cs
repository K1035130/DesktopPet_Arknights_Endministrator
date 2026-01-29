using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
public class DpWindowb : MonoBehaviour
{
    // ===== 窗口尺寸 =====
    public int windowWidth = 300;
    public int windowHeight = 300;


    // ===== 置顶 =====
    public bool alwaysOnTop = true;


    void Start()
    {
        // 固定窗口大小
        Screen.SetResolution(windowWidth, windowHeight, false);


#if UNITY_STANDALONE_WIN
        IntPtr hwnd = GetActiveWindow();


        // ===== 去边框 + 去标题栏 =====
        long style = GetWindowLongPtrSafe(hwnd, GWL_STYLE);
        style &= ~WS_BORDER;
        style &= ~WS_CAPTION;
        SetWindowLongPtrSafe(hwnd, GWL_STYLE, style);


        // ===== 透明分层窗口 =====
        long exStyle = GetWindowLongPtrSafe(hwnd, GWL_EXSTYLE);
        exStyle |= WS_EX_LAYERED;
        SetWindowLongPtrSafe(hwnd, GWL_EXSTYLE, exStyle);


        // ===== 置顶 =====
        if (alwaysOnTop)
        {
            SetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOMOVE | SWP_NOSIZE);
        }
#endif
    }


    void Update()
    {
#if UNITY_STANDALONE_WIN
        // ===== 原生系统级拖动（丝滑，无延迟） =====
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            IntPtr hwnd = GetActiveWindow();
            ReleaseCapture();
            SendMessage(hwnd, WM_NCLBUTTONDOWN, HTCAPTION, 0);
        }
#endif
    }


#if UNITY_STANDALONE_WIN
    // ================= Win32 API =================


    const int GWL_STYLE = -16;
    const int GWL_EXSTYLE = -20;


    const long WS_BORDER = 0x00800000L;
    const long WS_CAPTION = 0x00C00000L;
    const long WS_EX_LAYERED = 0x00080000L;


    static readonly IntPtr HWND_TOPMOST = new IntPtr(-1);


    const uint SWP_NOMOVE = 0x0002;
    const uint SWP_NOSIZE = 0x0001;


    const int WM_NCLBUTTONDOWN = 0x00A1;
    const int HTCAPTION = 0x0002;


    [DllImport("user32.dll")]
    static extern IntPtr GetActiveWindow();


    [DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
    static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);


    [DllImport("user32.dll", EntryPoint = "SetWindowLongPtr")]
    static extern IntPtr SetWindowLongPtr64(IntPtr hWnd, int nIndex, IntPtr dwNewLong);


    [DllImport("user32.dll")]
    static extern bool SetWindowPos(
    IntPtr hWnd,
    IntPtr hWndInsertAfter,
    int X,
    int Y,
    int cx,
    int cy,
    uint uFlags
    );


    [DllImport("user32.dll")]
    static extern bool ReleaseCapture();


    [DllImport("user32.dll")]
    static extern IntPtr SendMessage(
    IntPtr hWnd,
    int Msg,
    int wParam,
    int lParam
    );


    static long GetWindowLongPtrSafe(IntPtr hWnd, int nIndex)
    {
        return IntPtr.Size == 8
        ? GetWindowLongPtr64(hWnd, nIndex).ToInt64()
        : GetWindowLong32(hWnd, nIndex);
    }


    static IntPtr SetWindowLongPtrSafe(IntPtr hWnd, int nIndex, long value)
    {
        return IntPtr.Size == 8
        ? SetWindowLongPtr64(hWnd, nIndex, new IntPtr(value))
        : new IntPtr(SetWindowLong32(hWnd, nIndex, (int)value));
    }


    [DllImport("user32.dll", EntryPoint = "GetWindowLong")]
    static extern int GetWindowLong32(IntPtr hWnd, int nIndex);


    [DllImport("user32.dll", EntryPoint = "SetWindowLong")]
    static extern int SetWindowLong32(IntPtr hWnd, int nIndex, int dwNewLong);


#endif
}
