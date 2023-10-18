# Gogisoripper

A simple commandline tool to rip the CDDA tracks of iso files such as used by Good Old Games (GOG) to wav. Intended for use in installers for source ports of games that had CD audio to facilitate support for GOG source files during installation.

Usage:  gogisoripper.exe (input file path) (Output path & Filename) (track start position) (track end position)
  e.g.  gogisoripper.exe "D:\Games\GOG\Syndicate Wars\SWARS\game.gog" "C:\Program Files (x86)\Syndicate Wars\music\track1.wav" 456262128 556118288

You need to provide the track end and start positions in the iso file (usually called game.gog). Use a tool like isobuster to figure these out. 

Free to use and distribute, etc
