<%@ Page Language="C#" %>
<%@ Import Namespace="log4net" %>
<!DOCTYPE html>
<script runat="server">


    private void DivideByZero(int stackFrames = 10)
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
        var log = LogManager.GetLogger(GetType());

        try
        {
            DivideByZero();
        }
        catch (Exception exception)
        {
            log.Fatal("Unexpected Exception in Web Application", exception);
        }
    }


</script>
<html>
    <head runat="server">
        <title>SharpRaven.Log4Net.Tests.Web</title>
    </head>
    <body>
        <h1>Exception captured!</h1>
    </body>
</html>