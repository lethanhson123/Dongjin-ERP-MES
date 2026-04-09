public class AuthMiddleware
{
    private readonly RequestDelegate _next;
    private readonly CookieHelper _cookieHelper;

    public AuthMiddleware(RequestDelegate next, CookieHelper cookieHelper)
    {
        _next = next ?? throw new ArgumentNullException(nameof(next));
        _cookieHelper = cookieHelper ?? throw new ArgumentNullException(nameof(cookieHelper));
    }

    public async Task Invoke(HttpContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        var path = context.Request.Path.ToString();

        // Danh sách đường dẫn bỏ qua kiểm tra
        string[] allowedPaths = new[]
        {
            "/css", "/js", "/images", "/lib", "/favicon.ico",
            "/main_login", "/setup_form"
        };

        // Nếu đường dẫn bắt đầu bằng các path cho phép thì bỏ qua
        if (allowedPaths.Any(p => path.StartsWith(p, StringComparison.OrdinalIgnoreCase)))
        {
            await _next(context);
            return;
        }

        // Lấy cookie USER_IDX
        var userIdx = _cookieHelper.GetCookie(GlobalHelper.USER_IDX);

        // Kiểm tra cookie null/rỗng/không phải số hoặc <= 0
        if (string.IsNullOrWhiteSpace(userIdx) ||
            !int.TryParse(userIdx, out var userId) || userId <= 0)
        {
            // Xóa cookie cũ (nếu có)
            _cookieHelper.DeleteCookie(GlobalHelper.USER_IDX);

            // Redirect đến trang Login
            context.Response.Redirect("/MAIN_Login/Index");
            return;
        }

        // Nếu hợp lệ thì tiếp tục
        await _next(context);
    }
}
