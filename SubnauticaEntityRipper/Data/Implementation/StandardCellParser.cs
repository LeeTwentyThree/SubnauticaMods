using System;
using System.Collections.Generic;
using System.IO;
using SubnauticaEntityRipper.Data.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SubnauticaEntityRipper.Data.Implementation;

public class StandardCellParser : ICellParser, IDisposable
{
    private ProtobufSerializer _serializer;
    private Stream _stream;
    private bool _verbose;

    private Transform _tempTransform;

    public StandardCellParser(ProtobufSerializer serializer, Stream stream, bool verbose)
    {
        _serializer = serializer;
        _stream = stream;
        _verbose = verbose;
    }

    public IEnumerable<IEntityDefinition> ReadEntities()
    {
        if (!_serializer.TryDeserializeStreamHeader(_stream))
        {
            Plugin.Logger.LogError("Failed to deserialize header!");
            yield break;
        }

        if (_tempTransform == null)
        {
            _tempTransform = new GameObject("TempTransform").transform;
        }

        var loopHeader = ProtobufSerializer.loopHeaderPool.Get();
        loopHeader.Reset();

        _serializer.Deserialize(_stream, loopHeader, _verbose);
        var entities = loopHeader.Count;

        var componentHeader = ProtobufSerializer.componentHeaderPool.Get();
        var dictionary = ProtobufSerializer.componentCountersPool.Get();

        var gameObjectData = ProtobufSerializer.gameObjectDataPool.Get();
        CoreEntityData cellRootData = null;
        for (var i = 0; i < entities; i++)
        {
            gameObjectData.Reset();
            _serializer.Deserialize(_stream, gameObjectData, _verbose);

            // see ProtobufSerializer.DeserializeIntoGameObject
            loopHeader.Reset();
            _serializer.Deserialize(_stream, loopHeader, _verbose);
            var componentCount = loopHeader.Count;
            for (var j = 0; j < componentCount; j++)
            {
                componentHeader.Reset();
                dictionary.Clear();
                _serializer.Deserialize(_stream, componentHeader, _verbose);
                // bool isEnabled = componentHeader.IsEnabled;

                if (string.IsNullOrEmpty(componentHeader.TypeName)) continue;

                string typeName = componentHeader.TypeName;
                var cachedType = ProtobufSerializer.GetCachedType(typeName);
                int id = ProtobufSerializer.IncrementComponentCounter(dictionary, cachedType);

                /* Component orAddComponent = GetOrAddComponent(gameObject, cachedType, typeName, id, goData.CreateEmptyObject);
                    if (!orAddComponent)
                    {
                        Debug.LogWarningFormat(gameObject, "Serialized component '{0}' not found in game object", componentHeader.TypeName);
                        SkipDeserialize(stream);
                    }
                    else
                    {
                        Deserialize(stream, orAddComponent, cachedType, verbose > 2);
                    }
                    */

                // [TEMPORARY LOGIC]
                if (cachedType == typeof(Transform))
                {
                    _serializer.Deserialize(_stream, _tempTransform, _verbose);
                }
                else
                {
                    _serializer.SkipDeserialize(_stream);
                }

                // [END OF TEMPORARY LOGIC]

                // maybe useful later?
                // SetIsEnabled(orAddComponent, isEnabled);
            }

            CoreEntityData coreData;
            if (i == 0)
            {
                coreData = new CoreEntityData(gameObjectData.ClassId, gameObjectData.Id,
                    _tempTransform.position,
                    _tempTransform.rotation,
                    _tempTransform.lossyScale);
                cellRootData = coreData;
            }
            else if (cellRootData != null)
            {
                coreData = new CoreEntityData(gameObjectData.ClassId, gameObjectData.Id,
                    _tempTransform.position + cellRootData.Position,
                    _tempTransform.rotation,
                    _tempTransform.lossyScale);
            }
            else
            {
                Plugin.Logger.LogError("Stray entity found (no cell root)!");
                coreData = new CoreEntityData(gameObjectData.ClassId, gameObjectData.Id,
                    _tempTransform.position,
                    _tempTransform.rotation,
                    _tempTransform.lossyScale);
            }

            yield return new EntityDefinition(coreData);
        }

        ProtobufSerializer.gameObjectDataPool.Return(gameObjectData);
        ProtobufSerializer.loopHeaderPool.Return(loopHeader);
        ProtobufSerializer.componentHeaderPool.Return(componentHeader);
        ProtobufSerializer.componentCountersPool.Return(dictionary);
    }

    public void Dispose()
    {
        _stream?.Dispose();
        if (_tempTransform != null)
        {
            Object.Destroy(_tempTransform.gameObject);
        }
    }
}