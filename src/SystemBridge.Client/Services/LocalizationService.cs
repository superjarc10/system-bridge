using System.Net.Http.Json;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace SystemBridge.Client.Services;

public class LocalizationService
{
    private const string StorageKey = "systembridge_language";
    private readonly HttpClient _http;
    private readonly IJSRuntime _js;
    private Dictionary<string, string> _translations = new(StringComparer.OrdinalIgnoreCase);

    public LocalizationService(NavigationManager nav, IJSRuntime js)
    {
        _http = new HttpClient { BaseAddress = new Uri(nav.BaseUri) };
        _js = js;
    }

    public string CurrentCulture { get; private set; } = "en";
    public event Action? OnLanguageChanged;

    public IReadOnlyList<(string Code, string Name)> SupportedLanguages { get; } = new[]
    {
        ("en", "English 🇬🇧"),
        ("sl", "Slovenščina 🇸🇮"),
        ("de", "Deutsch 🇩🇪")
    };

    public async Task InitializeAsync()
    {
        try
        {
            var saved = await _js.InvokeAsync<string>("localStorage.getItem", StorageKey);
            if (!string.IsNullOrWhiteSpace(saved) && SupportedLanguages.Any(l => l.Code == saved))
            {
                CurrentCulture = saved;
            }
        }
        catch
        {
            CurrentCulture = "en";
        }

        await LoadLanguageAsync(CurrentCulture);
    }

    public async Task SetCultureAsync(string culture)
    {
        if (CurrentCulture == culture) return;
        CurrentCulture = culture;
        await LoadLanguageAsync(culture);

        try
        {
            await _js.InvokeVoidAsync("localStorage.setItem", StorageKey, culture);
        }
        catch
        {
            // Ignore browser storage write errors
        }

        OnLanguageChanged?.Invoke();
    }

    private async Task LoadLanguageAsync(string culture)
    {
        try
        {
            var data = await _http.GetFromJsonAsync<Dictionary<string, string>>($"lang/{culture}.json");
            if (data is not null)
            {
                _translations = new Dictionary<string, string>(data, StringComparer.OrdinalIgnoreCase);
            }
        }
        catch
        {
            if (culture != "en")
            {
                await LoadLanguageAsync("en");
            }
        }
    }

    public string this[string key]
    {
        get
        {
            if (string.IsNullOrWhiteSpace(key)) return string.Empty;
            return _translations.TryGetValue(key, out var val) ? val : key;
        }
    }

    public string Get(string key) => this[key];
}