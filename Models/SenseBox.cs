namespace westpac.Models;

public class SenseBox
{
    public required string Name { get; set; }

    public string? Description { get; set; }

    public string? GroupTag { get; set; }

    public required string Exposure { get; set; }

    public required Location Location { get; set; }

    public string? Model { get; set; }

    public Sensor[]? Sensors { get; set; }

    public string[]? SensorTemplates { get; set; }

    public MQTT? Mqtt { get; set; }

    public TTN? Ttn { get; set; }

    public bool? UseAuth { get; set; }

    public bool? SharedBox { get; set; }
}

public class Location
{
    public required int Lat { get; set; }
    public required int Lng { get; set; }
    public required int Height { get; set; }
}

public class Sensor
{
    public required string Title { get; set; }
    public required string Unit { get; set; }
    public required string SensonType { get; set; }
    public string icon { get; set; } = string.Empty;
}

public class MQTT
{
    public required bool Enabled { get; set; }
    public required string Url { get; set; }
    public required string Topic { get; set; }
    public required string MessageFormat { get; set; }
    public required string DecodeOptions { get; set; }
    public required string ConnectionOptions { get; set; }
}

public class TTN
{
    public required string Dev_id { get; set; }
    public required string App_id { get; set; }
    public required string Profile { get; set; }
    public string DecodeOptions { get; set; } = string.Empty;
    public int Port { get; set; } = 0;
}
