
using System.Net;
using System.Net.Mail;

namespace myProject.Middlewares;

public class Errors
{
    private RequestDelegate next;

    public Errors(RequestDelegate next)
    {
        //console.log("Error middleware created");
         System.Console.WriteLine("Error middleware created");
        this.next = next;
    }

    public async Task Invoke(HttpContext context)
    {
       //console.log("Error middleware invoked");
        System.Console.WriteLine("Error middleware invoked");
        context.Items["success"] = false;
        bool success = false;
        try
        {
            await next(context);
            context.Items["success"] = true;
        }
        catch (ApplicationException e)
        {
            context.Items["success"] = false;
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync(e.Message);
        }
        catch (Exception e)
        {
            context.Response.StatusCode = 500;
            await context.Response.WriteAsync($"{e.Message}__BIG error");
            // try{
            //         context.Response.StatusCode = 500;
            // //await context.Response.WriteAsync(e.Message);
            // String address = "gilli055672@gmail.com";
            // MailMessage m = new MailMessage();
            // m.From = new MailAddress("mk9743552@gmail.com");
            // m.To.Add(address);
            // m.Subject = "you have error in core";
            // m.Body = "check your error"+e.Message;
            // SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
            // smtpClient.Credentials = new NetworkCredential("gilli055672@gmail.com", "g_327741278");
            // smtpClient.EnableSsl = true;
            // }
            // catch(Exception ex){
            //     Console.WriteLine(ex.Message);
            // }

        }
    }
}
 public static partial class ErrorMiddelware{
    public static WebApplication UseError(this WebApplication app){
        app.UseMiddleware<Errors>();
        return app;
    }
 }

