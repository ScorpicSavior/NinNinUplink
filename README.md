# NinNinUplink

Nin-Nin Uplink Application

This program interconnects specific Tree of Savior Addons with an online
service at http://nin-nin.daijoubu.rocks/ in order to collect data, report
ingame events and otherwise enhance the gaming experience.

The online service is free for members of the Daijoubu team and friends.

## Installing and Running

Go to the [releases](https://github.com/ScorpicSavior/NinNinUplink/releases)
page on GitHub and download the zip bundle. Extract it into a folder anywhere
you like. Then simply launch NinNinUplink.exe and see what happens ;-)

The application will try to detect your Tree of Savior installation automatically,
so you don't need to set the ToS folder yourself. If the auto-detection fails,
you will be asked to point to the game folder.

You will need your Nin-Nin API Key for anything to work. [Get it here](http://nin-nin.daijoubu.rocks/api-key).

Depending on your version of Windows, you might need to install the
[.NET Framework 4.5 from Microsoft](https://go.microsoft.com/fwlink/?LinkID=324519).
Other required libraries are included.

## Building From Source

This application is written in C# (with .NET 4.5), so you need Microsoft Visual
Studio 2015 for Windows Desktop. It also makes use of several NuGet Packages;
the NuGet Package Manager comes pre-installed with VS2015, just make sure to
refresh the packages when opening the solution.

## License

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.

## Additional Disclaimer

This project is not affiliated with Tree of Savior / IMCGAMES CO., LTD. in any way.
