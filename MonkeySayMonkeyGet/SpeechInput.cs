namespace MonkeySayMonkeyGet;

public struct SpeechInput
{
    public string text;
    public Context context;

    public SpeechInput(string text, Context context)
    {
        this.text = text;
        this.context = context;
    }

    public enum Context
    {
        Partial,
        Final
    }

    public bool ContainsPhrase(string phrase)
    {
        return PhraseManager.ContainsPhrase(this, phrase);
    }
}