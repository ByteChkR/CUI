using System.Collections;
using System.Numerics;

using CUI.Common;
using CUI.Common.Components;
using CUI.Common.Components.Containers;
using CUI.Common.Components.Containers.Panels;
using CUI.Common.Drawing;
using CUI.Common.Rendering;

namespace CUI.Console;

internal class Program
{
    private static RenderContext RenderContext { get; } = new RenderContext(new ConsoleRenderer());

    private static IEnumerator MoveTest(Renderable renderable, bool even, string tag, Text log)
    {
        Vector2 offset = new Vector2(1, even ? 1 : -1);
        for (int i = 0; i < 10; i++)
        {
            renderable.Transform.Position += offset;
            renderable.Transform.ZIndex = i % 2 == 0 == even ? 1 : 0;
            log.Value += $"{tag} new Positon: {renderable.Transform.Position} with Z-Index: {renderable.Transform.ZIndex}\n";
            if(i % 2 == 0 == even)
            {
                renderable.BackgroundColor = RenderColor.Red;
                renderable.ForegroundColor = RenderColor.Green;
            }
            else
            {
                renderable.BackgroundColor = RenderColor.Green;
                renderable.ForegroundColor = RenderColor.Red;
            }

            yield return null;
        }
    }

    private static void Loop(params IEnumerator[] animations)
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
            RenderContext.Render();
            Thread.Sleep(100);
        }
    }
    
    private static void Main(string[] args)
    {
        Renderable text1 = new BorderedPanel(BorderOptions.Default)
            .WithChild(new Text("Hello World!")
                           .WithSize(12, 1)
                      )
            .WithPadding(3, 1)
            .WithSize(6, 1)
            .WithColors(RenderColor.Red, RenderColor.Green)
            .WithOverflow(OverflowMode.Hidden);
        Renderable text2 = new BorderedPanel(BorderOptions.Double)
            .WithChild(new Text("Hello World!\nThis is a test!")
                           .WithLayout(LayoutMode.Fit)
                      )
            .AtPosition(5, 10)
            .WithPadding(3, 1)
            .WithSize(6, 1)
            .WithColors(RenderColor.Green, RenderColor.Red)
            .WithLayout(LayoutMode.Fit);

        Text log = new ScrollingText().WithLayout(LayoutMode.Fill);

        Renderable test = new VerticalLayout()
            .WithChildren(
                          new Renderable()
                              .WithChildren(text1, text2)
                              .AsLayoutElement(2)
                              .WithLayout(LayoutMode.FillX),
                          new BorderedPanel(BorderOptions.Double)
                              .WithSize(1, 1)
                              .WithLayout(LayoutMode.FillX)
                              .WithChild(log)
                         )
            .WithLayout(LayoutMode.Fill);
        
        RenderContext.Elements.AddChild(test);
        RenderContext.Render();
        Loop(MoveTest(text1, true, "Text1", log), MoveTest(text2, false, "Text2", log));
    }
}