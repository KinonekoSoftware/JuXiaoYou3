# 关于RenderTargetBitmap

1. Border不能设置Margin，不然会有截图偏移的问题
2. Border内部不能有Border，不然会有截图偏移的问题

# 关于动画

如果多个事件会对于一个公共控件访问，就会出问题

这时候务必在Code中实现动画



示例代码：Drawer.cs