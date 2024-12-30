using System.Collections;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

using CUI.Common;
using CUI.Common.Components;
using CUI.Common.Components.Containers;
using CUI.Common.Components.Containers.Panels;
using CUI.Common.Drawing;
using CUI.Common.Rendering;
using CUI.Common.Serializer;

namespace CUI.Console;

internal class Program
{

    private static IEnumerator MoveTest(Renderable renderable, bool even, string tag, Text log)
    {
        bool reverse = false;
        while (true)
        {
            Vector2 offset = new Vector2(1, even ? 1 : -1);
            if(reverse)
            {
                offset *= -1;
            }
            for (int i = 0; i < 10; i++)
            {
                renderable.Transform.Position += offset;
                renderable.Transform.ZIndex = i % 2 == 0 == even ? 1 : 0;
                log.Value += $"{tag} new Positon: {renderable.Transform.Position} with Z-Index: {renderable.Transform.ZIndex}\n";
                

                yield return null;
            }
            reverse = !reverse;
        }
    }

    private static void Loop(RenderContext context, params IEnumerator[] animations)
    {
        bool completed = false;

        while (!completed)
        {
            completed = true;

            for (int i = 0; i < animations.Length; i++)
            {
                IEnumerator animation = animations[i];
                if (animation.MoveNext())
                {
                    completed = false;
                }
            }
            context.Render();
            Thread.Sleep(250);
        }
    }

    private static void TestLayout(RenderContext context)
    {
        Renderable? text1 = context.Root.Find("Text1");
        Renderable? text2 = context.Root.Find("Text2");
        Text? log = context.Root.Find<Text>("Log");
        
        if(text1 is null || text2 is null || log is null)
        {
            throw new Exception("Could not find all required elements");
        }
        
        Loop(context,MoveTest(text1, true, "Text1", log), MoveTest(text2, false, "Text2", log));
    }

    private static void Chat(RenderContext context)
    {
        Text? chatWindow = context.Root.Find<Text>("ChatWindow");
        TextInput? chatInput = context.Root.Find<TextInput>("ChatInput");
        
        if(chatWindow is null || chatInput is null)
        {
            throw new Exception("Could not find all required elements");
        }


        bool exit = false;
        chatInput.OnKeyPress += args =>
        {
            if (args.Key.Key == ConsoleKey.Enter && args.Key.Modifiers == 0)
            {
                chatWindow.Value += $"[{DateTime.Now:HH:mm:ss}] Tim(tim@byt3.dev): {chatInput.Value}\n";
                chatInput.Value = string.Empty;
                chatInput.CursorPosition = 0;
                args.Handled = true;
                chatInput.SendChangedEvent(chatInput);
            }
            else if(args.Key.Key == ConsoleKey.Escape)
            {
                exit = true;
                args.Handled = true;
            }
        };
        chatInput.OnChanged += _ => context.Render();
        while (!exit)
        {
            Thread.Sleep(250);
        }
    }
    
    private static void Main(string[] args)
    {

        if (args[0]
            .EndsWith("TestLayout.xml"))
        {
            using RenderContext context = new RenderContext(new ConsoleRenderer());
            context.Root = LayoutSerializer.FromFile(args[0]);
            context.Render();
            TestLayout(context);
        }
        else if(args[0]
            .EndsWith("Chat.xml"))
        {
            using RenderContext context = new RenderContext(new ConsoleRenderer(), true);
            context.Root = LayoutSerializer.FromFile(args[0]);
            context.Render();
            Chat(context);
        }
        else if (args[0]
                 .EndsWith("Console.xml"))
        {
            RenderContext context = new RenderContext(new ConsoleRenderer(), true);
            context.Root = LayoutSerializer.FromFile(args[0]);
            TextInput? input = context.Root.Find<TextInput>("Input");
            Text? output = context.Root.Find<Text>("Output");
            if (input is null || output is null)
            {
                throw new Exception("Could not find all required elements");
            }

            using CommandWindow window = new CommandWindow(context, output, input);
            window.Run();
        }
        Environment.Exit(0);
    }
}