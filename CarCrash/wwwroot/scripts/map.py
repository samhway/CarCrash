import requests
import xml.etree.ElementTree as ET
import sys

def map_function(StartLL, EndLL):
    StartLL = StartLL
    EndLL = EndLL
    RouteNodeLL = []

    route = requests.get(f'http://router.project-osrm.org/route/v1/driving/{StartLL};{EndLL}?alternatives=false&annotations=nodes')
    routejson = route.json()
    route_nodes = routejson['routes'][0]['legs'][0]['annotation']['nodes']


    if len(route_nodes) > 50 :
        x = len(route_nodes) / 49
        x = round(x)

    new_nodes = []
    c = 0
    while c < len(route_nodes):
        new_nodes.append(route_nodes[c])
        c += x

    for node in new_nodes:
        response_xml = requests.get(f'https://api.openstreetmap.org/api/0.6/node/{node}')
        response_xml_as_string = response_xml.content
        responseXml = ET.fromstring(response_xml_as_string)
        for child in responseXml.iter('node'):
            RouteNodeLL.append((float(child.attrib['lat']), float(child.attrib['lon'])))

    return(RouteNodeLL)

# StartLL = '-111.672670,40.240383'
# EndLL = '-113.562143,37.102946'
StartLL = sys.argv[1]
EndLL = sys.argv[2]

list = map_function(StartLL, EndLL)

print(list)