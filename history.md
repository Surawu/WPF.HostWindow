`[1.100.0.0]` - `[2018.12.10]`
==========================

* Conclude(): 
	- 将WinForm控件寄宿到WPF Grid中可以显示；但是消息会先走WPF，之后在流到WinForm控件。
	- 无法将SubWindow在主窗体显示，提示VisualTarget 的根 Visual 不能具有父级。
	- 在主窗体显示的WPF控件（TextBox等）无法编辑。
	- 多文档系统MDI和WPF.HostWindow的实现不一致
		1. MDI是通过Control或UserControl实现的
	- 需求：两个Window， 两个线程，两个消息循环，cEditor直接集成到Ncstudio会卡住主线程，所以让一个线程独自处理nceditor的绘图，
		   另一个线程负责ncstudio的加工
