using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PdaUpgradeCards.Data;

public class MusicPlaylist
{
    private readonly PdaMusic[] _defaultTracks;
    private readonly PdaMusic[] _arrangedTracks;

    public MusicPlaylist(IEnumerable<PdaMusic> tracks)
    {
        var tracksArray = tracks as PdaMusic[] ?? tracks.ToArray();
        var count = tracksArray.Length;
        _defaultTracks = new PdaMusic[count];
        _arrangedTracks = new PdaMusic[count];
        for (int i = 0; i < tracksArray.Length; i++)
        {
            _defaultTracks[i] = tracksArray[i];
            _arrangedTracks[i] = tracksArray[i];
        }
    }
    
    public IReadOnlyList<PdaMusic> Tracks => _shuffled ? _arrangedTracks : _defaultTracks;
    public int Size => _defaultTracks.Length;

    private bool _shuffled;

    public void Shuffle()
    {
        int n = _arrangedTracks.Length;  
        while (n > 1) {  
            n--;  
            int k = Random.Range(0, n + 1);  
            (_arrangedTracks[k], _arrangedTracks[n]) = (_arrangedTracks[n], _arrangedTracks[k]);
        }

        _shuffled = true;
    }

    public void ResetToDefaultOrder()
    {
        _shuffled = false;
    }

    public bool TryGetTrackNumber(PdaMusic music, out int trackNumber)
    {
        trackNumber = -1;
        for (int i = 0; i < Size; i++)
        {
            if (music == Tracks[i])
                trackNumber = i;
        }

        return trackNumber >= 0;
    }
}