﻿import * as d3 from "d3-selection";
var dotSrcLines;
var dotSrc = `
digraph {
    graph [label="Click on a node or an edge to delete it" labelloc="t", fontsize="20.0" tooltip=" "]
    node [style="filled"]
    Node1 [id="NodeId1" label="N1" fillcolor="#d62728"]
    Node2 [id="NodeId2" label="N2" fillcolor="#1f77b4"]
    Node3 [id="NodeId3" label="N3" fillcolor="#2ca02c"]
    Node4 [id="NodeId4" label="N4" fillcolor="#ff7f0e"]
    Node1 -> Node2 [id="EdgeId12" label="E12"]
    Node1 -> Node3 [id="EdgeId131" label="E13"]
    Node2 -> Node3 [id="EdgeId23" label="E23"]
    Node3 -> Node4 [id="EdgeId34" label="E34" tooltip=" its vary important!!!"]
}
`;


var graphviz = d3.select("#graph").graphviz();

export function render(dotSrc) {
    console.log('DOT source =', dotSrc);
    dotSrcLines = dotSrc.split('\n');

    graphviz
        .transition(function () {
            return d3.transition()
                .delay(100)
                .duration(1000);
        })
        .renderDot(dotSrc)
        .on("end", interactive);
}

function interactive() {

    console.log('aaa');
    console.log(d3.selectAll('.node').size());
    nodes1 = d3.selectAll('.node,.edge');
    console.log(nodes1);
//    if(nodes)
    nodes
        .on("click", function () {
            /*var title = d3.select(this).selectAll('title').text().trim();
            console.log(title);
            var class1 = d3.select(this).attr('class');
            console.log(class1);
            var text = d3.select(this).selectAll('text').text();
            console.log(text);*/
            var id = d3.select(this).attr('id');
            console.log(id);
            /*var color = d3.select(this).attr('color');
            console.log(color);*/
            let event = new Event("setDotObject", { id_object: id });
            document.dispatchEvent(event);


            /*        dotElement = title.replace('->',' -> ');
                    console.log('Element id="%s" class="%s" title="%s" text="%s" dotElement="%s"', id, class1, title, text, dotElement);
                    console.log('Finding and deleting references to %s "%s" from the DOT source', class1, dotElement);
                    for (i = 0; i < dotSrcLines.length;) {
                        if (dotSrcLines[i].indexOf(dotElement) >= 0) {
                            console.log('Deleting line %d: %s', i, dotSrcLines[i]);
                            dotSrcLines.splice(i, 1);
                        } else {
                            i++;
                        }
                    }
                    dotSrc = dotSrcLines.join('\n');
                    render(dotSrc);*/
        });
}

//render(dotSrc);
/*window.chrome.webview.addEventListener('message', event => {
    alert(event.data);
    WriteDataFromCsharp(event.data);
});*/
export function addCustomEventListener(dotNetObjectRef) {
    document.addEventListener('setDotObject', (event) => {
        // Calls a method by name with the [JSInokable] attribute (above)
        dotNetObjectRef.invokeMethodAsync('OnCustomEvent')
    });
}