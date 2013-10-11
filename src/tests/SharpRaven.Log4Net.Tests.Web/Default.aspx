<%@ Page Language="C#" %>
<%@ Import Namespace="log4net" %>
<%@ Import Namespace="log4net.Config" %>
<!DOCTYPE html>
<script runat="server">


    private static void DivideByZero(int stackFrames = 10)
    {
        if (stackFrames == 0)
        {
            var a = 0;
            var b = 1 / a;
        }
        else
            DivideByZero(--stackFrames);
    }


    private void Page_Load(object sender, EventArgs e)
    {
        // TODO: Figure out why this is needed (the assembly attribute in AssemblyInfo.log4net.cs doesn't work). [asbjornu]
        XmlConfigurator.Configure();

        var log = LogManager.GetLogger(GetType());

        try
        {
            DivideByZero();
        }
        catch (Exception exception)
        {
            log.Error("Unexpected Exception in Web Application", exception);
        }
    }


</script>
<html>
    <head runat="server">
        <title>SharpRaven.Log4Net.Tests.Web</title>
    </head>
    <body>
        <h1>Exception captured!</h1>
        <form method="post">
            <input type="hidden" name="Hidden">
            <input type="submit" name="Button" value="Submit">
        </form>
    </body>
</html>