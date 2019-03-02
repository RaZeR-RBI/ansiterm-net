import json
import string

with open('colors.json') as f:
    data = json.load(f)

color_map = []
color_names = []

for color in data:
    NAME = color["name"].rstrip(string.digits) + str(color["colorId"])
    RGB = color["rgb"]
    COLOR_STR = "Color.FromArgb({}, {}, {})".format(RGB["r"], RGB["g"], RGB["b"])
    color_names.append(NAME)
    color_map.append(COLOR_STR)

print("public enum Color256 {")
for name in color_names:
    print("\t" + name + ",")
print("}")

print("internal static Color[] s_indexedColors = [")
for color in color_map:
    print("\t" + color + ",")
print("];")