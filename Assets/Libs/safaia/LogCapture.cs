using UnityEngine;

public static class LogCapture
{
	public static event Application.LogCallback LogCallback = delegate { }; // ログが出力された時に呼び出されます

	/// <summary>
	/// コールバックを登録するコンストラクタ
	/// </summary>
	static LogCapture ()
	{
		Application.RegisterLogCallback (HandleLog);
	}

	/// <summary>
	/// ログが出力された時に呼び出されます
	/// </summary>
	private static void HandleLog (string condition, string stackTrace, LogType type)
	{
		LogCallback (condition, stackTrace, type);
	}
}
