# 76-108 MHz RDS Stereo FM Transmitter
<br>The transmitter is a low-power signal generator, in the VHF-FM frequency range of 76..108MHz. The presented device has the following features:

- Low output power level (~ 20mW)
- Frequency: 76MHz - 108Mhz,
- Tuning step: 50kHz,
- Preemphasis 50uS/75uS,
- FM deviation step adjustment
- Transmitting in Stereo/Mono mode,
- Support for analog sound (Chinch inputs),
- Support for SPDiF digital sound (Fiber optic / Chinch input),
- USB-C socket for controlling RDS / transmitter parameters,
- Windows application to set transmitter parameters and RDS support:
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- PI: Station ID,
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- PTY: Station program type,
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Changing the broadcast type (PTY) to "NEWS" during the broadcast of the News.
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- PS: Radio station name,
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- MS: Music/Speech broadcasts,
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- TP/TA: Traffic announcement support.
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- Date / Time - synchronization of the radio receiver with the PC controlling the transmitter
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- RadioText (Static): Text broadcast without control application,
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- RadioText (Dynamic): Text broadcast depending on the type of broadcast (Music/Speech/Traffic).
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;- RadioText automation (Music): Work with an external player (Winamp / Foobar2000) to automate RadioText
<br>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; (e.g., "Now Playing: Artist - Track").

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Transmitter_Front.jpg "FM Transmitter front view")

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Transmitter_Back.jpg "WFM Transmitter rear view")

### RF parameters:
The following are the results of the RF test of the transmitter

Signal spectrum, and output power measurement (20mW):
![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/pomiar2.jpeg "WFM Transmitter rear view")

Ten consecutive harmonics and the measurement of their attenuation:
![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/pomiar3.jpeg "WFM Transmitter rear view")

FM modulation - the amount of frequency deviation and audio distortion
![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/pomiar6.jpeg "WFM Transmitter rear view")

### Device main screen:
When the device is turned on, the screen will display a welcome message, followed by the main screen:

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Device_MainScreen.png "Device Main Screen")

At the top of the screen is the name of the device: "VHF FM TRANSMITTER".
<br>Below in large font is displayed the station name - (RDS: PS). The station name can contain up to 8 characters (RDS standard specification). NOTE - the name is shown on the display regardless of whether RDS broadcasting is enabled.
<br>If RDS broadcasting is enabled - below the name, the content of the currently broadcast RadioText is displayed. If the length of the message does not fit the width of the screen - it is scrolled to allow control of the entire content.

The functions used and settings are visualized on the screen with icons, the description of which is shown in the table:
<table>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/analog.png" /></td><td>Analog input active</td></tr>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/digital.png" /></td><td>SPDiF digital input active</td></tr>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/mono.png" /></td><td>Broadcasting in mono mode</td></tr>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/stereo.png" /></td><td>Broadcasting in stereo mode</td></tr>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/USBico2.png" /></td><td>USB cable plugged in</td></tr>
<tr><td><img src="https://github.com/kuczy/FM-Transmitter/blob/master/_images/RDSico.png" /></td><td>RDS broadcasting on</td></tr>
</table>

<p>The choice of signal source (analog/SPDiF) is made automatically. By default, the analog source is selected (chinch on the back of the chassis: RIGHT/LEFT). After connecting a digital source (COAXIAL/OPTO fiber) - the device will recognize whether the data is correct, and then automatically switch from analog to digital source. Do not connect two digital sources - this will cause a data error and switch to an analog source.
<br>The mono/stereo transmission mode depends on the device's settings.
<br>USB icon appears on the screen when the cable is connected to the rear USB-C socket. No icon - function inactive
<br>RDS broadcast icon depends on the device settings. No icon - function inactive

At the very bottom of the screen, a black box displays the broadcast frequency

There is a dial on the front panel of the device, which allows you to navigate through the menu of the transmitter.
- Press the knob like a button to access the transmitter's settings menu
- The screen will change to menu screen.
- Rotation of the knob allows you to move through successive parameters (up down) - which is visualized by a marker (#) next to the selected parameter.
- Pressing the knob at the selected parameter - go to edit the parameter value (the # marker will change position moving to the right at the value).
- Turn the knob to the right or left to set the desired parameter.
- Confirm the parameter by clicking on the knob again - The parameter will be stored and the # marker will return to the parameter selection.

### Transmitter settings screen (#1):
<table>
<tr><td><- Back</td><td>Exit the menu</td></tr>
<tr><td>Frequancy</td><td>Transmit frequency (76..108MHz in 50kHz increments)</td></tr>
<tr><td>RDS</td><td>ON/OFF</td></tr>
<tr><td>Audio mode</td><td>STEREO/MONO</td></tr>
<tr><td>Pre emphase</td><td>50/75 uS</td></tr>
<tr><td>Deviation</td><td>FM Deviation 0..87 kHz</td></tr>
<tr><td>More -></td><td>Next menu</td></tr>
</table>

### Transmitter settings screen (#2):
<table>
<tr><td><- Back</td><td>Return to previous menu</td></tr>
<tr><td>Pilot level</td><td>19kHz Pilot level 7/8/9/10 %</td></tr>
<tr><td>RDS deviat.</td><td>RDS Deviation 0..44.4 kHz</td></tr>
</table>

## Windows Application (Version 1.1)
The application is available for download here: [DOWNLOAD APPLICATION](https://github.com/kuczy/FM-Transmitter/raw/master/Download/FM%20Transmitter.exe)
<br>To take advantage of all the capabilities of the FM transmitter, use the Windows application "FM Transmitter".
It allows you to set the parameters of the transmitter but most importantly, it allows you to dynamically control RDS messages.
<p>Two clocks are shown at the bottom of the application window. 
<br>Verify the correctness of the displayed date and the universal time (UTC) and local time (Local). The date and time is sent by RDS, and allows synchronization of listeners' radio receivers.

### Application main screen:
---
The main screen is divided into three panels:

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Windows_App1.1.jpg "Windows Application Main Screen")

<p><b>"Front Panel".</b>
<br>Is a representation of the physical display of the device. All icons, stand for the same functions described earlier when describing the device.</p>

<p><b>"RDS Control"</b>
<br>RDS Standard Allows you to specify the type of broadcast currently being transmitted. We distinguish between two basic types of transmission (M/S): Music / Speech.  The radio receiver can change the sound correction settings depending on the type of broadcast.
<br>Additional special transmission types are "NEWS" - changing the PTY parameter set in the settings tab - to "NEWS" when broadcasting news sudsion, and "Traffic information" (TA) .
The possibility of broadcasting traffic information (TP) depends on the setting in the Settings tab. The radio receiver can automatically select the radio station broadcasting the traffic message, and/or increase the volume of the received message.</p>

<p>The RDS Control panel allows manual switching of the broadcast type controls.
Clicking on the Music/Speech controls toggles the basic broadcast types, while clicking on the "Traffic information - Broadcasting now" control. - has higher priority over the basic types. As long as it is unchecked - switching between Music and Speech will not change the broadcast type.</p>
<p>At the bottom of the panel, a black box shows the currently selected broadcast type.</p>

<p><b>"RDS Radio Text"</b>
This panel is responsible for "Radio Text" content. The specificity of RDS allows broadcasting text messages with a maximum length of 64 characters, without national dialect characters.
Three text fields allow you to enter separate messages for each type of broadcast (Music/Speech/News/Traffic).</p>
<p>In addition - to make it easier to control the RDS - if after typing a message, you confirm it by pressing the "ENTER" key - the broadcast type automatically changes to the one corresponding to the field.
In this case, the "News/Traffic" type has no priority over "Music/Speech."<p>
<p>Another feature of the transmitter application is the ability to automate RadioText broadcast messages for "Music" type broadcasts.
To use the automation, check the "Music broadcasting Radio test source: Automatic" control. In this case, the message content will be taken from a "buffer" - text file "RDS_Automation.txt" which will be created automatically after the first launch of the application in its root folder. To make the automation work properly - I suggest using an audio player: Winamp along with the plug-in: Winamp Now Playing To File, or Foobar2000 , along with the plug-in: Now Playing Simple.</p>

<p>&nbsp;</p>
<p><b>Winamp player / Winamp Now Playing To File.</b>

[Download Winamp latest version](https://download.nullsoft.com/winamp/client/winamp_latest_full.exe)

[Download "Winamp Now Playing To File" plugin](https://github.com/kuczy/FM-Transmitter/raw/master/Download/WinampNowPlayingToFile.zip)
<p>After installing Winamp, install the plugin according to the author's description on the site: https://github.com/Aldaviva/WinampNowPlayingToFile
<p></p>Select the plug-in from the Plig-ins/General Purpose menu, then press "Configure selected plug-in", And configure according to your preferences. example:
  
![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Winamp_1.jpg "Plugin configuration 1")

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Winamp_2.jpg "Plugin configuration 2")

<p>&nbsp;</p>
<p><b>Foobar2000 player (32bit) / Now Playing Simple.</b>

[Download 32 bit version Foobar2000](https://www.foobar2000.org/download)

[Download "Now Playing Simple" plugin](https://github.com/kuczy/FM-Transmitter/raw/master/Download/foo_np_simple.dll)

To automate RDS RadioText with Foobar2000 player, you need to install the 32bit version of the player. NOTE: The 64-bit version unfortunately does not work properly with the "Now Playing Simple" plug-in.
After installation, go to the installation folder (typically: "C:\Program Files (x86)\Foobar2000\components") where you should place the plug-in file: "foo_np_simple.dll".

Then configure the plugin.
1. show the path to the file "RDS_Automation.txt",
2. set Encoding to UTF-8,
3. enter the correct "Formatting string". In the example, the prefix "Now Playing" is used. - you can use your
4. set Events to "On new track". - writing to the file will take place when the track changes
5. clear the "On exit" field

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Foobar2000_1.jpg "Plugin configuration")

A preview of the RadioText formatted this way is marked with a green square. The plugin configured this way will keep the uploaded RDS RadioText up to date.</p>

### Settings Screen
---
The Settings tab is used to set some parameters of the transmitter, and to configure the RDS.
<br>Some of the parameters are repeated from the settings on the device - they are synchronized with each other when the application is launched. These options have been added, for the convenience of operating the transmitter.

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Windows_App2.1.jpg "Windows Application Settings Screen")

Transmitter settings:

- Station name: An 8-character string used as the PS parameter of the RDS system, and also as the name displayed on the device's screen,
- Frequency: Selection of the transmission frequency. When the application is turned on, the current value stored in the device will be highlighted (selected),
- Audio mode: MONO/STEREO,
- Pre emphase: 50/75 uS,
- RDS: ON/OFF

RDS Settings:

- Programme Identification (PI): This is the unique 4 character hexadecimal code that identifies the station. Every station in a country should use a unique 3 character code with the correct country prefix character.
- PTY Standard: The list of program types differs between Europe and the US. Select the correct region to correctly select the program type (PTY) below
- ProgrammeType (PTY): This coding of up to 31 pre-defined programme types allows users to find similar programming by genre
- Primarily Broadcasts: (M/S): The given value is transmitted when the transmitter is not connected via USB. (When the transmitter is connected to the computer - the parameter is controlled by the application).
- Traffic Programme (TP): Used to allow the user to find only those stations that regularly broadcast traffic bulletins. Disabling this option deactivates the ability to check "Traffic information - broadcasting now".
- Static Radio Text: Allows transmission of a fixed radio text phrase when the transmitter is not connected via USB

To have the settings stored in the transmitter, after configuration, press the "Save to device" button

### About Screen
---
Brief information about the program version, QR code to github resources

![alt text](https://github.com/kuczy/FM-Transmitter/blob/master/_images/Windows_App3.1.jpg "Windows Application About Screen")
