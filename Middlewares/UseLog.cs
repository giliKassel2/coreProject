
using System.Diagnostics;

namespace myProject.Middlewares;

public class UseLog
{
    private RequestDelegate next;

    public UseLog(RequestDelegate next)
    {
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
        // System.Console.WriteLine("in log");
        var stoper = new Stopwatch();
        stoper.Start();
        await next(context);
        
        string filePath = Path.Combine(Directory.GetCurrentDirectory(), "GILI_LOG/log.txt");
        if (!File.Exists(filePath))
        {
            using (StreamWriter writer = new StreamWriter(filePath, true))
            {
                writer.WriteLine("log file created");
            }
        }
        try{
        using (StreamWriter writer = new StreamWriter(filePath, true))
        {
            string status = context.Response.StatusCode.ToString();
            System.Console.WriteLine($" log : {context.Request.Path}.{context.Request.Method} took {stoper.ElapsedMilliseconds}ms."
            +$"status:{status} Success: {context.Items["success"]}");
            writer.WriteLine($"{context.Request.Path}.{context.Request.Method} took {stoper.ElapsedMilliseconds}ms. "
            +$"status:{status}  Success: {context.Items["success"]}");
         
        }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error writing to log file: {ex.Message}");
        }
    }
}
public static class logMiddleware
{
    public static WebApplication UseLog(
        this WebApplication app)
    {
        app.UseMiddleware<UseLog>();
        return app;
    }
}
