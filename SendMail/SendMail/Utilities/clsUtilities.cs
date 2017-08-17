using System.Windows.Forms;

namespace SendMail
{
    public class clsUtilities
    {
        public static void ShowMessageInformation(string msg)
        {
            MessageBox.Show(msg, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void ShowMessageError(string msg)
        {
            MessageBox.Show(msg, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ShowMessageWarning(string msg)
        {
            MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static DialogResult ShowQuestionMessage(string msg)
        {
            return MessageBox.Show(msg, "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        }
    }
}
