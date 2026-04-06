function checkQuiz() {
  const answers = document.getElementsByName("q1");
  let selected = null;

  for (let i = 0; i < answers.length; i++) {
    if (answers[i].checked) {
      selected = answers[i].value;
    }
  }

  if (selected === "b") {
    alert("Richtig! Container nutzen den Host-Kernel.");
    localStorage.setItem("quiz1", "passed");
  } else {
    alert("Falsch. Versuch es nochmal!");
  }
}
