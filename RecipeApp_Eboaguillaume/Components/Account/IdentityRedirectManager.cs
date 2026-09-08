using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Components;

namespace RecipeApp_Eboaguillaume.Components.Account;

internal sealed class IdentityRedirectManager(NavigationManager navigationManager)
{
    [DoesNotReturn]
    public void RedirectTo(string? uri)
    {
        uri ??= "";
        var uriWithoutQuery = navigationManager.ToAbsoluteUri(uri).GetLeftPart(UriPartial.Path);
        navigationManager.NavigateTo(uriWithoutQuery, forceLoad: true);
        throw new InvalidOperationException($"{nameof(IdentityRedirectManager)} can only be used during static rendering.");
    }
}
