using UniElLib;

namespace ParserLibrary;

[Annotation("Пустой Sender")]
public class NullSender:Sender
{
    public override TypeContent typeContent => TypeContent.internal_list;
    //   public string IDResponsedReceiverStep = "";

}