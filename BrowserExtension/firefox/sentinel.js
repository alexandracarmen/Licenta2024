browser.contextMenus.create({
  id: "1",
  title: "Predict truth of selection",
  contexts: ["selection"],
});

browser.contextMenus.onClicked.addListener((e) => {
  console.log(e)
  fetch("http://localhost:5247/extension", {
  method: "POST",
  body: JSON.stringify({text: e.selectionText}),
  headers: {
    "Content-Type": "application/json"
  }
})
  .then((response) => response.json().then((json) => {
    browser.tabs.create({url: "http://localhost:5247/news-prediction/" + json.id});
  }))
});