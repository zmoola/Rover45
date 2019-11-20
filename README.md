# Rover45

The aim of this project is to navigate a character, in this case a Mars Rover, about a grid zone as defined by the user.
The user is required to input the dimensions of the grid, the starting position of the Rover, and a list of commands for the Rover to perform.

<h2>Design</h2>
<p>In response to the brief, the following assumptions were made with regards to the program:</p>
<ul>
  <li>The commands will be given as individual inputs, in the order of zone size, starting location, and then movement commands.</li>
  <li>The maximum zone size is 9x9.</li>
  <li>Co-ordinates are supplied as a 2-digit value, which will be split to form the X and Y co-ordinates respectively.</li>
  <li>Cardinal directions are supplied as the initial alphabet in uppercase.</li>
  <li>Move commands are limited to M,R,and L, supplied in uppercase.</li>
</ul>

<p>In addition, I thought it made sense to add the following functions:</p>
<ul>
  <li>The ability to still execute commands from your new position.</li>
  <li>The ability to query your position.</li>
  <li>The ability to dispose the current zone and start over.</li>
  <li>The ability to exit the program via command.</li>
</ul>

<h2>Functionality</h2>
<p>
    When the program is run, the user is greeted with a summary of the additional commands available, then prompted to input the commands for the zone size, starting location and direction, and the movement commands in their respective order. Each input is validated according to what is expected in that context. These include:
  
<ul>
  <li>The zone size should only be supplied as a single, 2-digit value.</li>
  <li>The starting location and direction are given as a single command, separated by a space or tab.</li>
  <li>The starting location should be a single, 2-digit value.</li>
  <li>The starting direction should be a single uppercase character correspoding to an initial of the cardinal directions.</li>
  <li>The movement command list should only comprise of the letters M,L, and R, without whitespace, in any order or frequency.</li>
  <li>The movement command list should not take the Rover outside the zone boundaries at any time.</li>
</ul>

</p>

<p>
  The addtional commands for the program are case insensitive.These commands are:
  
<ul>
  <li>'New' or 'Reset' clears the stored values and allows you to capture commands from scratch.</li>
  <li>'Position' or 'Pos' displays the Rover's current position and direction.</li>
  <li>'Exit or 'X' quits the application.</li>
</ul>
</p>


<h2>Execution</h2>
<p>
  I had decided to create the program as a console application, with a structure emulating a basic shell application.
  <br/>
  I added prompts for the required commands in order to provide the user with some feedback. There are validations on the differet commands depending on what is required of them.
 </p>
  <p>
  Given the scale of the application, I avoided over-engineering the logic and decided to use a relatively flat structure.
  I thought it simplest to interpret the cardinal points as decimal fractions, which are aligned to the string values by a disctionary local to the application scope.
  The co-ordinates and zone sizes are stored as local integer values. The idea being that the curent values are adjusted on a move depending on which direction the Rover is facing.
 </p>
  <p>
  For reading the movement commands, I've opted to go with iterating through the string and qualifying the command by use of a switch statement. This is because, while brainstorming this interaction, the actual processing cost of this method versus parsing and mapping the string prior to processing seemed to favor the iterative method. Though it's not the most sleek, The objective is accomplished in a reliable and repeatable fashion.
</p>

<p>
  The program is built a a C# console application using .Net Core v2.1. The solution can be downloaded and compiled locally.
</p>
<p>
  For the sake of simplicity, the project was tested manually running through multiple scenarios and verifying that the results were as expected.
</p>
