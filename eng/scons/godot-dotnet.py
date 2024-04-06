import os, sys, platform

from SCons.Variables import EnumVariable, PathVariable, BoolVariable
from SCons.Variables.BoolVariable import _text2bool
from SCons.Tool import Tool
from SCons.Builder import Builder
from SCons.Errors import UserError
from SCons.Script import ARGUMENTS
