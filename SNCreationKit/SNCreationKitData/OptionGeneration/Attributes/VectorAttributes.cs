namespace SNCreationKitData.OptionGeneration.Attributes;

public class Vector2FieldAttribute(string name, string description) : TupleAttribute(name, description, typeof(float), 2);
public class Vector3FieldAttribute(string name, string description) : TupleAttribute(name, description, typeof(float), 3);
public class Vector4FieldAttribute(string name, string description) : TupleAttribute(name, description, typeof(float), 4);
public class QuaternionFieldAttribute(string name, string description) : TupleAttribute(name, description, typeof(float), 4);