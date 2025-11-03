namespace FilmOfTheDay.Web.Models.Post;

public class ConfirmPopupViewModel
{
    public string PopupId { get; set; } = "confirmModal";
    public string Title { get; set; } = "Confirm Action";
    public string Message { get; set; } = "Are you sure you want to continue?";
    public string YesText { get; set; } = "Yes";
    public string NoText { get; set; } = "No";
    public string YesAction { get; set; } = "";
    public string? RedirectAftersuccess { get; set; }
}