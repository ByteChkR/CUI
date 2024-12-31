using System;
using System.IO;
using System.Numerics;
using System.Text;
using System.Xml;

using CUI.Common.Components;
using CUI.Common.Components.Containers;
using CUI.Common.Components.Containers.Panels;
using CUI.Common.Drawing;

namespace CUI.Common.Serializer;

public static class LayoutSerializer
{
        
    private static T ParseFlagsEnum<T>(string enumStr) where T : Enum
    {
        int result = 0;
        foreach (string part in enumStr.Split('|'))
        {
            object value = Enum.Parse(typeof(T), part, true);
            result |= (int)value;
        }
        return (T)(object)result;
    }
        
    private static Vector2 ParseVector(string vectorStr)
    {
        string[]? parts = vectorStr.Split(' ');
        if(parts.Length == 1)
        {
            return new Vector2(float.Parse(parts[0]));
        }

        return new Vector2(float.Parse(parts[0]), float.Parse(parts[1]));
    }

    private static Offset ParseOffset(string offsetStr)
    {
        string[]? parts = offsetStr.Split(' ');
        if(parts.Length == 1)
        {
            return new Offset(int.Parse(parts[0]));
        }

        if(parts.Length == 2)
        {
            return new Offset(int.Parse(parts[0]), int.Parse(parts[1]));
        }

        if(parts.Length == 4)
        {
            return new Offset(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]), int.Parse(parts[3]));
        }

        throw new Exception("Invalid offset format: " + offsetStr);
    }
    private static void ReadAttributes(Renderable target, XmlNode reader)
    {
        string? name = reader.Attributes?.GetNamedItem("Name")?.Value;
        string? layout = reader.Attributes?.GetNamedItem("Layout")?.Value;
        string? active = reader.Attributes?.GetNamedItem("Active")?.Value;
        string? pivot = reader.Attributes?.GetNamedItem("Pivot")?.Value;
        string? position = reader.Attributes?.GetNamedItem("Position")?.Value;
        string? innerSize = reader.Attributes?.GetNamedItem("Size")?.Value;
        string? padding = reader.Attributes?.GetNamedItem("Padding")?.Value;
        string? zIndex = reader.Attributes?.GetNamedItem("ZIndex")?.Value;
        string? overflow = reader.Attributes?.GetNamedItem("Overflow")?.Value;
        string? backgroundColor = reader.Attributes?.GetNamedItem("Background")?.Value;
        string? foregroundColor = reader.Attributes?.GetNamedItem("Color")?.Value;
        string? focusBackgroundColor = reader.Attributes?.GetNamedItem("FocusBackground")?.Value;
        string? focusForegroundColor = reader.Attributes?.GetNamedItem("FocusColor")?.Value;
        string? focusable = reader.Attributes?.GetNamedItem("Focusable")?.Value;
        string? captureControl = reader.Attributes?.GetNamedItem("CaptureControl")?.Value;
        string? tabIndex = reader.Attributes?.GetNamedItem("TabIndex")?.Value;
        target.Name = name ?? "";
        if (layout != null)
        {
            target.LayoutMode = ParseFlagsEnum<LayoutMode>(layout);
        }

        if (active != null)
        {
            target.Transform.Active = bool.Parse(active);
        }

        if (pivot != null)
        {
            target.Transform.Pivot = ParseVector(pivot);
        }

        if (position != null)
        {
            target.Transform.Position = ParseVector(position);
        }

        if (innerSize != null)
        {
            target.Transform.InnerSize = ParseVector(innerSize);
        }

        if (padding != null)
        {
            target.Transform.Padding = ParseOffset(padding);
        }

        if (zIndex != null)
        {
            target.Transform.ZIndex = int.Parse(zIndex);
        }

        if (overflow != null)
        {
            target.OverflowMode = Enum.Parse<OverflowMode>(overflow, true);
        }

        if (backgroundColor != null)
        {
            target.BackgroundColor = Enum.Parse<RenderColor>(backgroundColor, true);
        }

        if (foregroundColor != null)
        {
            target.ForegroundColor = Enum.Parse<RenderColor>(foregroundColor, true);
        }
        
        if (focusBackgroundColor != null)
        {
            target.FocusBackgroundColor = Enum.Parse<RenderColor>(focusBackgroundColor, true);
        }
        
        if (focusForegroundColor != null)
        {
            target.FocusForegroundColor = Enum.Parse<RenderColor>(focusForegroundColor, true);
        }
        
        if (focusable != null)
        {
            target.Focusable = bool.Parse(focusable);
        }
        
        if (captureControl != null)
        {
            target.CaptureControlKeys = bool.Parse(captureControl);
        }
        
        if (tabIndex != null)
        {
            target.TabIndex = int.Parse(tabIndex);
        }
    }

    private static string NormalizeText(string text)
    {
        // remove leading and trailing whitespace, remove duplicate whitespace
        string[]? lines = text.Trim()
                              .Replace("\r\n", "\n")
                              .Split('\n');
        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < lines.Length; i++)
        {
            string line = lines[i];
            string clean = line.Trim();

            if (!string.IsNullOrEmpty(clean))
            {
                if (i != lines.Length - 1)
                {
                    sb.AppendLine(clean);
                }
                else
                {
                    sb.Append(clean);
                }
            }
        }

        return sb.ToString();
    }
    private static Renderable ReadText(XmlNode reader)
    {
        Text text = new Text();

        text.Value = NormalizeText(reader.InnerText);
        ReadAttributes(text, reader);
        return text;
    }
        
    private static Renderable ReadScrollingText(XmlNode reader)
    {
        ScrollingText text = new ScrollingText();

        text.Value = NormalizeText(reader.InnerText);
        ReadAttributes(text, reader);
        return text;
    }
    private static Renderable ReadTextInput(XmlNode reader)
    {
        TextInput text = new TextInput();

        text.Value = NormalizeText(reader.InnerText);
        ReadAttributes(text, reader);
        return text;
    }
        
    public static Renderable ReadHorizontalLayout(XmlNode reader)
    {
        HorizontalLayout layout = new HorizontalLayout();
        ReadAttributes(layout, reader);
        return layout;
    }

    public static Renderable ReadVerticalLayout(XmlNode reader)
    {
        VerticalLayout layout = new VerticalLayout();
        ReadAttributes(layout, reader);
        return layout;
    }
        
    public static Renderable ReadPanel(XmlNode reader)
    {
        Panel panel = new Panel();
        ReadAttributes(panel, reader);
        return panel;
    }
        
    public static Renderable ReadLayoutElement(XmlNode reader)
    {
        if(reader.ChildNodes.Count != 1)
        {
            throw new Exception("LayoutElement must have exactly one child");
        }

        Renderable child = ReadNode(reader.ChildNodes[0]);
        LayoutElement panel = new LayoutElement(child);
        ReadAttributes(panel, reader);
        return panel;
    }
        
    public static Renderable ReadBorderedPanel(XmlNode reader)
    {
        string? border = reader.Attributes?.GetNamedItem("Border")?.Value;
        BorderedPanel panel = new BorderedPanel(BorderOptions.Borders[border ?? "Default"]);
        ReadAttributes(panel, reader);
        return panel;
    }

    private static Renderable ReadNode(XmlNode reader)
    {
        string nodeType = reader.Name;
        Renderable node;
        if(nodeType == "Text")
        {
            node = ReadText(reader);
        }
        else if (nodeType == "ScrollingText")
        {
            node = ReadScrollingText(reader);
        }
        else if (nodeType == "HorizontalLayout")
        {
            node = ReadHorizontalLayout(reader);
        }
        else if (nodeType == "VerticalLayout")
        {
            node = ReadVerticalLayout(reader);
        }
        else if (nodeType == "Panel")
        {
            node = ReadPanel(reader);
        }
        else if (nodeType == "BorderedPanel")
        {
            node = ReadBorderedPanel(reader);
        }
        else if (nodeType == "LayoutElement")
        {
            node = ReadLayoutElement(reader);
        }
        else if(nodeType == "TextInput")
        {
            node = ReadTextInput(reader);
        }
        else
        {
            throw new Exception("Unknown node type: " + nodeType);
        }
            
        string? weight = reader.Attributes?.GetNamedItem("Weight")?.Value;
        string? ignoreLayout = reader.Attributes?.GetNamedItem("IgnoreLayout")?.Value;
        string? fixedSize = reader.Attributes?.GetNamedItem("FixedSize")?.Value;
        if (weight != null || ignoreLayout != null || fixedSize != null)
        {
            if(node is not LayoutElement elem)
            {
                node = elem = new LayoutElement(node);
            }

            if(weight != null)
            {
                elem.Weight = float.Parse(weight);
            }

            if(ignoreLayout != null)
            {
                elem.IgnoreLayout = bool.Parse(ignoreLayout);
            }
            
            if(fixedSize != null)
            {
                elem.FixedSize = bool.Parse(fixedSize);
            }
        }
        
        if(node is not LayoutElement)
        {
            InnerRead(node, reader);
        }

        return node;
    }

    private static void InnerRead(Renderable result, XmlNode root)
    {
        foreach (XmlNode node in root.ChildNodes)
        {
            if (node.NodeType != XmlNodeType.Element)
            {
                continue;
            }

            Renderable child = ReadNode(node);
            result.AddChild(child);
        }
    }
    public static Renderable FromFile(string path)
    {
        XmlDocument reader = new XmlDocument();
        reader.Load(path);
        XmlElement root = reader.DocumentElement!;
            
        return ReadNode(root);
    }
    public static Renderable Parse(Stream s)
    {
        XmlDocument reader = new XmlDocument();
        reader.Load(s);
        XmlElement root = reader.DocumentElement!;
            
        return ReadNode(root);
    }
        
        
    public static Renderable Parse(string content)
    {
        XmlDocument reader = new XmlDocument();
        reader.LoadXml(content);
        XmlElement root = reader.DocumentElement!;
        return ReadNode(root);
    }
        
}