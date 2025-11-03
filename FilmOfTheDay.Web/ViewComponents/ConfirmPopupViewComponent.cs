using Microsoft.AspNetCore.Mvc;
using FilmOfTheDay.Web.Models.Post;

namespace FilmOfTheDay.Web.ViewComponents;
public class ConfirmPopupViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(ConfirmPopupViewModel model)
    {
        model.PopupId ??= "confirmModal";
        model.Title ??= "Confirm Action";
        model.Message ??= "Are you sure you want to continue?";
        model.YesText ??= "Yes";
        model.NoText ??= "No";
        return View(model);
    }
}
