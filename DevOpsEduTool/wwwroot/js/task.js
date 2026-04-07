function checkQuiz(quizId, correctAnswer) {
    const form = document.getElementById(quizId);
    if (!form) {
        console.error("Form not found" + quizId);
        return;
    }

    const feedback = document.getElementById("feedback-quiz-" + quizId);
    if (!feedback) {
        console.error("Could not find feedback field: feedback-quiz-" + quizId);
        return;
    }

    const selected = form.querySelector('input[type="radio"]:checked');

    if (!selected) {
        feedback.innerHTML = "Please select an answer before checking!";
        feedback.style.color = "orange";
        return;
    }

    if (selected.value === correctAnswer) {
        feedback.innerHTML = "<strong>Correct!</strong> Well done. You've understood this concept.";
        feedback.style.color = "#42b883";
    } else {
        let hint = "Try again! ";
        // specific hints based on quiz topic
        if (quizId.includes('devops-1')) hint += "DevOps is about culture and automation, not replacing people.";
        if (quizId.includes('devops-2')) hint += "Docker is software/tooling, not hardware.";
        if (quizId === 'docker-1') hint += "Containers are not full machines; they share the host's underlying resources to stay lightweight.";
        if (quizId === 'docker-2') hint += "Think of a library or storage facility where images are kept for everyone to download.";
        if (quizId === 'cicd-1') hint += "CI focuses on the early stage: getting everyone's code together and checking if it works.";
        if (quizId === 'cicd-2') hint += "Continuous Deployment goes one step further than Delivery. It's about getting the code to the user automatically.";
        

        feedback.innerHTML = "<strong>" + hint + "</strong>";
        feedback.style.color = "#ff4757";
    }
}
function runSimulation(stepId, nextId, message) {
    const current = document.getElementById(stepId);
    const log = document.getElementById("pipeline-log");

    current.disabled = true;
    current.innerText = "Running...";
    current.style.opacity = "0.7"; 

    setTimeout(() => {
        const entry = document.createElement("p");
        entry.innerText = "SUCCESS: " + message;
        entry.style.color = "#42b883";
        entry.style.margin = "5px 0";
        log.appendChild(entry);

        log.scrollTop = log.scrollHeight;

        current.innerText = "Completed";
        current.style.background = "#2d3748"; 
        current.style.opacity = "1";

        if (nextId) {
            const next = document.getElementById(nextId);
            next.style.display = "inline-block";
        } else {
            document.getElementById("finish-sim").style.display = "block";
        }
    }, 1500);
}

function checkBasicsFlow() {
    const p = document.getElementById("flow-plan").value;
    const b = document.getElementById("flow-build").value;
    const t = document.getElementById("flow-test").value;
    const d = document.getElementById("flow-deploy").value;
    const feedback = document.getElementById("feedback-basics-flow");

    
    if (p === "1" && b === "2" && t === "3" && d === "4") {
        feedback.innerHTML = "<strong>Perfect!</strong> You understood the logical flow of a DevOps pipeline.";
        feedback.style.color = "#42b883";
    } else {
        feedback.innerHTML = "<strong>Not quite.</strong> Hint: You must <em>Plan</em> before you code, and <em>Test</em> before you <em>Deploy</em> to the customer.";
        feedback.style.color = "#ff4757";
    }
}

function nextStep(stepNumber) {
  const steps = document.querySelectorAll(".step");

  steps.forEach((step) => {
    step.style.display = "none";
  });

  const currentStep = document.getElementById("step-" + stepNumber);
  if (currentStep) {
    currentStep.style.display = "block";
    window.scrollTo(0, 0);
  }
}
