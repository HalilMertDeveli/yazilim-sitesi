using Microsoft.AspNetCore.Html;

namespace Portfolio.Services;

/// <summary>Small inline SVG icon set (24px grid, stroke-based) so the page needs no icon font.</summary>
public static class Icons
{
    private static HtmlString Stroke(string paths, string extraClass = "") =>
        new($"<svg class=\"icon {extraClass}\" viewBox=\"0 0 24 24\" width=\"18\" height=\"18\" fill=\"none\" stroke=\"currentColor\" stroke-width=\"1.75\" stroke-linecap=\"round\" stroke-linejoin=\"round\" aria-hidden=\"true\" focusable=\"false\">{paths}</svg>");

    public static HtmlString ArrowRight => Stroke("<path d=\"M5 12h14\"/><path d=\"m13 6 6 6-6 6\"/>");
    public static HtmlString ArrowUpRight => Stroke("<path d=\"M7 17 17 7\"/><path d=\"M8 7h9v9\"/>");
    public static HtmlString Mail => Stroke("<rect x=\"3\" y=\"5\" width=\"18\" height=\"14\" rx=\"2\"/><path d=\"m3 7 9 6 9-6\"/>");
    public static HtmlString Lock => Stroke("<rect x=\"4\" y=\"11\" width=\"16\" height=\"10\" rx=\"2\"/><path d=\"M8 11V7a4 4 0 0 1 8 0v4\"/>");
    public static HtmlString Star => Stroke("<path d=\"m12 3 2.7 5.6 6.1.9-4.4 4.3 1 6.1L12 17l-5.4 2.9 1-6.1L3.2 9.5l6.1-.9z\"/>");
    public static HtmlString Check => Stroke("<path d=\"m5 12 5 5 9-10\"/>");
    public static HtmlString Circle => Stroke("<circle cx=\"12\" cy=\"12\" r=\"7\" stroke-dasharray=\"3 3\"/>");
    public static HtmlString Copy => Stroke("<rect x=\"9\" y=\"9\" width=\"12\" height=\"12\" rx=\"2\"/><path d=\"M5 15V5a2 2 0 0 1 2-2h10\"/>");
    public static HtmlString MapPin => Stroke("<path d=\"M12 21s-7-6.2-7-12a7 7 0 0 1 14 0c0 5.8-7 12-7 12z\"/><circle cx=\"12\" cy=\"9\" r=\"2.5\"/>");
    public static HtmlString Menu => Stroke("<path d=\"M4 8h16\"/><path d=\"M4 16h16\"/>");

    public static HtmlString GitHub => new(
        "<svg class=\"icon\" viewBox=\"0 0 24 24\" width=\"18\" height=\"18\" fill=\"currentColor\" aria-hidden=\"true\" focusable=\"false\"><path d=\"M12 2C6.48 2 2 6.58 2 12.23c0 4.52 2.87 8.35 6.84 9.7.5.1.68-.22.68-.49l-.01-1.7c-2.78.62-3.37-1.37-3.37-1.37-.46-1.18-1.11-1.5-1.11-1.5-.91-.64.07-.62.07-.62 1 .07 1.53 1.06 1.53 1.06.9 1.57 2.35 1.12 2.92.85.09-.66.35-1.12.63-1.37-2.22-.26-4.56-1.14-4.56-5.07 0-1.12.39-2.04 1.03-2.76-.1-.26-.45-1.3.1-2.71 0 0 .84-.28 2.75 1.05a9.4 9.4 0 0 1 5 0c1.91-1.33 2.75-1.05 2.75-1.05.55 1.41.2 2.45.1 2.71.64.72 1.03 1.64 1.03 2.76 0 3.94-2.34 4.8-4.57 5.06.36.32.68.94.68 1.9l-.01 2.81c0 .27.18.6.69.49A10.1 10.1 0 0 0 22 12.23C22 6.58 17.52 2 12 2z\"/></svg>");

    public static HtmlString LinkedIn => new(
        "<svg class=\"icon\" viewBox=\"0 0 24 24\" width=\"18\" height=\"18\" fill=\"currentColor\" aria-hidden=\"true\" focusable=\"false\"><path d=\"M20.45 20.45h-3.56v-5.57c0-1.33-.02-3.04-1.85-3.04-1.86 0-2.14 1.45-2.14 2.95v5.66H9.35V9h3.41v1.56h.05c.48-.9 1.64-1.85 3.37-1.85 3.6 0 4.27 2.37 4.27 5.46v6.28zM5.34 7.43a2.06 2.06 0 1 1 0-4.13 2.06 2.06 0 0 1 0 4.13zM7.12 20.45H3.56V9h3.56v11.45zM22.22 0H1.77C.79 0 0 .77 0 1.73v20.54C0 23.23.79 24 1.77 24h20.45c.98 0 1.78-.77 1.78-1.73V1.73C24 .77 23.2 0 22.22 0z\"/></svg>");
}
