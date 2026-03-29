function completeModule(moduleKey) {
  localStorage.setItem(moduleKey, "completed");
  updateProgress();
  alert("Module finished! Returning to dashboard.");
  window.location.href = "../frontend/index.html";
}

function updateProgress() {
  let completedModules = 0;
  const totalModules = 3;

  if (localStorage.getItem("module_docker") === "completed") {
    completedModules++;
  }

  if (localStorage.getItem("module_cicd") === "completed") {
    completedModules++;
  }

  if (localStorage.getItem("module_devopsBasics") === "completed") {
    completedModules++;
  }

  const progressBar = document.querySelector(".progress-bar");
  if (progressBar) {
    let percent = (completedModules / totalModules) * 100;
    progressBar.style.width = percent + "%";
    progressBar.innerText = Math.round(percent) + "%";
  }
  const progressText = document.getElementById("progress-text");
  if (progressText) {
    progressText.innerText = `You have completed ${completedModules} out of ${totalModules} modules.`;
  }
}

window.onload = updateProgress;
