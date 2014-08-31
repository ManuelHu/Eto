using System.Collections.Generic;
using Eto.Drawing;
using System;

namespace Eto.Forms
{
	/// <summary>
	/// Represents a display on the system.
	/// </summary>
	[Handler(typeof(Screen.IHandler))]
	public class Screen : Widget
	{
		new IHandler Handler { get { return (IHandler)base.Handler; } }

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.Screen"/> class.
		/// </summary>
		public Screen()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.Screen"/> class with the specified <paramref name="handler"/>.
		/// </summary>
		/// <remarks>
		/// Used by platform implementations to create instances of a screen with a particular handler.
		/// </remarks>
		/// <param name="handler">Handler for the screen.</param>
		public Screen(IHandler handler)
			: base(handler)
		{
		}

		#pragma warning disable 612,618

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.Screen"/> class.
		/// </summary>
		/// <param name="generator">Generator.</param>
		[Obsolete("Use default constructor instead")]
		public Screen(Generator generator)
			: base(generator, typeof(IHandler))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="Eto.Forms.Screen"/> class.
		/// </summary>
		/// <param name="generator">Generator.</param>
		/// <param name="handler">Handler.</param>
		[Obsolete("Use Screen(IScreen) instead")]
		public Screen(Generator generator, IHandler handler)
			: base(generator, handler)
		{
		}

		#pragma warning restore 612,618

		/// <summary>
		/// Gets an enumerable of display screens available on the current system.
		/// </summary>
		/// <value>The screens of the current system.</value>
		public static IEnumerable<Screen> Screens
		{
			get
			{
				var handler = Platform.Instance.CreateShared<IScreensHandler>();
				return handler.Screens;
			}
		}

		/// <summary>
		/// Gets the display bounds of all screens on the current system.
		/// </summary>
		/// <value>The display bounds of all screens.</value>
		public static RectangleF DisplayBounds
		{
			get
			{
				var handler = Platform.Instance.CreateShared<IScreensHandler>();
				var bounds = RectangleF.Empty;
				foreach (var screen in handler.Screens)
				{
					bounds.Union(screen.Bounds);
				}
				return bounds;
			}
		}

		/// <summary>
		/// Gets the primary screen of the current system.
		/// </summary>
		/// <remarks>
		/// This is typically the user's main screen.
		/// </remarks>
		/// <value>The primary screen.</value>
		public static Screen PrimaryScreen
		{
			get
			{
				var handler = Platform.Instance.CreateShared<IScreensHandler>();
				return handler.PrimaryScreen;
			}
		}

		/// <summary>
		/// Gets the logical Dots/Pixels Per Inch of the screen.
		/// </summary>
		/// <value>The Dots Per Inch</value>
		public float DPI { get { return Handler.Scale * 72f; } }

		/// <summary>
		/// Gets the logical scale of the pixels of the screen vs. points.
		/// </summary>
		/// <remarks>
		/// The scale can be used to translate points to pixels.  E.g.
		/// <code>
		/// var pixels = points * screen.Scale;
		/// </code>
		/// This is useful when creating fonts that need to be a certain pixel size.
		/// 
		/// Since this is a logical scale, this will give you the 'recommended' pixel size that will appear to be the same
		/// physical size, even on retina displays.
		/// </remarks>
		/// <value>The logical scale of pixels per point.</value>
		public float Scale { get { return Handler.Scale; } }

		/// <summary>
		/// Gets the real Dots/Pixels Per Inch of the screen, accounting for retina displays.
		/// </summary>
		/// <remarks>
		/// This is similar to <see cref="DPI"/>, however will give you the 'real' DPI of the screen.
		/// For example, a Retina display on OS X will have the RealDPI twice the DPI reported.
		/// </remarks>
		/// <value>The real DP.</value>
		public float RealDPI { get { return Handler.RealScale * 72f; } }

		/// <summary>
		/// Gets the real scale of the pixels of the screen vs. points.
		/// </summary>
		/// <remarks>
		/// The scale can be used to translate points to 'real' pixels.  E.g.
		/// <code>
		/// var pixels = points * screen.Scale;
		/// </code>
		/// This is useful when creating fonts that need to be a certain pixel size.
		/// 
		/// Since this is a real scale, this will give you the actual pixel size. 
		/// This means on retina displays on OS X will appear to be half the physical size as regular displays.
		/// </remarks>
		/// <value>The real scale of pixels per point.</value>
		public float RealScale { get { return Handler.RealScale; } }

		/// <summary>
		/// Gets the bounds of the display in the <see cref="DisplayBounds"/> area.
		/// </summary>
		/// <remarks>
		/// The primary screen's upper left corner is always located at 0,0.
		/// A negative X/Y indicates that the screen location is to the left or top of the primary screen.
		/// A positive X/Y indicates that the screen location is to the right or bottom of the primary screen.
		/// </remarks>
		/// <value>The display's bounds.</value>
		public RectangleF Bounds { get { return Handler.Bounds; } }

		/// <summary>
		/// Gets the working area of the display, excluding any menu/task bars, docks, etc.
		/// </summary>
		/// <remarks>
		/// This is useful to position your window in the usable area of the screen.
		/// </remarks>
		/// <value>The working area of the screen.</value>
		public RectangleF WorkingArea { get { return Handler.WorkingArea; } }

		/// <summary>
		/// Gets the number of bits each pixel uses to represent its color value.
		/// </summary>
		/// <value>The screen's bits per pixel.</value>
		public int BitsPerPixel { get { return Handler.BitsPerPixel; } }

		/// <summary>
		/// Gets a value indicating whether this screen is the primary/main screen.
		/// </summary>
		/// <value><c>true</c> if this is the primary screen; otherwise, <c>false</c>.</value>
		public bool IsPrimary { get { return Handler.IsPrimary; } }

		/// <summary>
		/// Handler interface for the <see cref="Screen"/>.
		/// </summary>
		public new interface IHandler : Widget.IHandler
		{
			/// <summary>
			/// Gets the logical scale of the pixels of the screen vs. points.
			/// </summary>
			/// <remarks>
			/// This scale should be based on a standard 72dpi screen.
			/// </remarks>
			/// <value>The logical scale of pixels per point.</value>
			float Scale { get; }

			/// <summary>
			/// Gets the real scale of the pixels of the screen vs. points.
			/// </summary>
			/// <remarks>
			/// This should be based on a standard 72dpi screen, and is useful for retina displays when the real DPI
			/// is double the logical DPI.  E.g. 1440x900 logical screen size is actually 2880x1800 pixels.
			/// </remarks>
			/// <value>The real scale of pixels per point.</value>
			float RealScale { get; }

			/// <summary>
			/// Gets the number of bits each pixel uses to represent its color value.
			/// </summary>
			/// <value>The screen's bits per pixel.</value>
			int BitsPerPixel { get; }

			/// <summary>
			/// Gets the bounds of the display.
			/// </summary>
			/// <remarks>
			/// The primary screen's upper left corner is always located at 0,0.
			/// A negative X/Y indicates that the screen location is to the left or top of the primary screen.
			/// A positive X/Y indicates that the screen location is to the right or bottom of the primary screen.
			/// </remarks>
			/// <value>The display's bounds.</value>
			RectangleF Bounds { get; }

			/// <summary>
			/// Gets the working area of the display, excluding any menu/task bars, docks, etc.
			/// </summary>
			/// <remarks>
			/// This is useful to position your window in the usable area of the screen.
			/// </remarks>
			/// <value>The working area of the screen.</value>
			RectangleF WorkingArea { get; }

			/// <summary>
			/// Gets a value indicating whether this screen is the primary/main screen.
			/// </summary>
			/// <value><c>true</c> if this is the primary screen; otherwise, <c>false</c>.</value>
			bool IsPrimary { get; }
		}

		/// <summary>
		/// Handler interface for static methods of the <see cref="Screen"/>.
		/// </summary>
		public interface IScreensHandler
		{
			/// <summary>
			/// Gets an enumerable of display screens available on the current system.
			/// </summary>
			/// <value>The screens of the current system.</value>
			IEnumerable<Screen> Screens { get; }

			/// <summary>
			/// Gets the primary screen of the current system.
			/// </summary>
			/// <remarks>
			/// This is typically the user's main screen.
			/// </remarks>
			/// <value>The primary screen.</value>
			Screen PrimaryScreen { get; }
		}
	}
}

